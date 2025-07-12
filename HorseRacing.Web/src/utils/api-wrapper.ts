/** Все 2 и более слэшей подряд кроме тех, перед которыми стоит : */
const safeMultipleSlashesRegex = /(?<!:)\/{2,}/g;
/** Убирает лишние слэши в строке */
export function fixUrlSlashes(str: string) {
    return str.replace(safeMultipleSlashesRegex, '');
}

type CommonRequestHeadersList = 'Content-Type' | 'Content-Length' | 'Authorization' | 'Accept' | 'User-Agent' | 'Content-Encoding';
type CommonMethodsList = 'POST' | 'GET' | 'UPDATE' | 'DELETE';

type OtherString = (string & {});

type ConfigHeaders = Partial<
    Record<CommonRequestHeadersList | OtherString, string>
>;

// eslint-disable-next-line @typescript-eslint/no-explicit-any
type RequestConfig<T = any> = {
    baseUrl: string;
    body: T;
    method: CommonMethodsList | OtherString;
    headers: ConfigHeaders;
    requestMiddlewares: RequestMiddleware[],
    responseMiddlewares: ResponseMiddleware[],
};

type MiddlewareNext = () => Promise<void>;
type Middleware<T> = (data: T, next: MiddlewareNext) => void | Promise<void>;

export type RequestMiddleware = Middleware<RequestConfig>;
export type ResponseMiddleware = Middleware<Response>;

export type ApiWrapper = {
    postJson<T>(path: string, config?: Partial<RequestConfig<T>>): Promise<Response>;
    /** Ответ будет сохранен в памяти до перезагрузки страницы.
     * При вызове при наличии сохраненного ответа, возвращает его копию */
    postOnce(path: string, config?: Partial<RequestConfig<never>>): Promise<Response>;
    postForm(path: string, config?: Partial<RequestConfig<FormData>>): Promise<Response>;
    extend: (config: Partial<RequestConfig>) => ApiWrapper;
};

function mergeConfigs(defaultConfig: Readonly<RequestConfig>, ...configs: Partial<Readonly<RequestConfig>>[]): RequestConfig {
    return configs.reduce<RequestConfig>((prev, curr) => {
        return {
            baseUrl: curr.baseUrl ?? prev.baseUrl,
            body: curr.body ?? prev.body,
            method: curr.method ?? prev.method,
            headers: { ...prev.headers, ...curr.headers },
            requestMiddlewares: prev.requestMiddlewares.concat(curr.requestMiddlewares ?? []),
            responseMiddlewares: prev.responseMiddlewares.concat(curr.responseMiddlewares ?? []),
        };
    }, defaultConfig);
}

function prepareRequestInit(config: RequestConfig): RequestInit {
    const headers = Object.entries(config.headers ?? {})
        .filter((x): x is [string, string] => x[1] != undefined);

    return {
        method: config.method,
        body: config.body,
        headers,
    };
}

async function runMiddlewares<T>(data: T, middlewares: Middleware<T>[]) {
    const usedIndexes = new Set<number>();
    async function run(index: number) {
        const middleware = middlewares[index];
        if (!middleware) {
            return;
        }
        if (usedIndexes.has(index)) {
            console.error('next() called multiple times in', middleware.name);
            return;
        }
        usedIndexes.add(index);
        await middleware(data, () => run(index + 1));
    }
    await run(0);
}

const cache = new Map<string, Response>();
function getCacheKey(path: string, config: RequestConfig) {
    return config.method + config.baseUrl + path;
}

export function makeApiWrapper(
    initialConfig: Partial<RequestConfig>,
): ApiWrapper {
    const baseConfig: RequestConfig = {
        baseUrl: fixUrlSlashes(initialConfig.baseUrl ?? ''),
        body: initialConfig.body,
        headers: initialConfig.headers ?? {},
        method: initialConfig.method ?? 'GET',
        requestMiddlewares: initialConfig.requestMiddlewares ?? [],
        responseMiddlewares: initialConfig.responseMiddlewares ?? [],
    };

    async function execute(path: string, config: RequestConfig) {
        const url = [config.baseUrl, path].filter(Boolean).join('/');
        await runMiddlewares(config, config.requestMiddlewares);
        const response = await fetch(url, prepareRequestInit(config));
        await runMiddlewares(response, config.responseMiddlewares);
        return response;
    }

    const postJson: ApiWrapper['postJson'] = async (path, payload = {}) => {
        const config = mergeConfigs(
            baseConfig,
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
            },
            payload,
            {
                requestMiddlewares: [
                    (config, next) => {
                        config.body = JSON.stringify(config.body);
                        return next();
                    }
                ]
            }
        );
        return execute(path, config);
    };

    const postOnce: ApiWrapper['postOnce'] = async (path, payload = {}) => {
        const config = mergeConfigs(
            baseConfig,
            {
                method: 'POST'
            },
            payload,
        );
        const cacheKey = getCacheKey(path, config);
        const cachedResponse = cache.get(cacheKey);
        if (cachedResponse) {
            return cachedResponse.clone();
        }
        const response = await execute(path, config);
        cache.set(cacheKey, response);
        return response.clone();
    };

    const postForm: ApiWrapper['postForm'] = async (path, payload = {}) => {
        const config = mergeConfigs(
            baseConfig,
            {
                method: 'POST',
            },
            payload,
        );
        return execute(path, config);
    };

    const extend: ApiWrapper['extend'] = (config) => {
        return makeApiWrapper(mergeConfigs(baseConfig, config));
    };

    return { postJson, postOnce, postForm, extend };
}

export const apiWrapper = makeApiWrapper({ baseUrl: '/api/v1/' });
