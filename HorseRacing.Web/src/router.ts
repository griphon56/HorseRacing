import { createRouter, createWebHistory } from 'vue-router';
import { routerAuthGuard } from '~/core/auth';
import { RouteName } from '~/interfaces';
import { MainLayout } from '~/components/layout';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/auth',
            name: RouteName.Auth,
            component: () => import('~/pages/auth/auth-page.vue')
        },
        {
            path: '/',
            component: MainLayout,
            children: [
                {
                    path: '/account',
                    name: RouteName.Account,
                    component: () => import('~/pages/user/account-page.vue'),
                },
                {
                    path: '/profile',
                    name: RouteName.Profile,
                    component: () => import('~/pages/user/profile-page.vue'),
                },
                {
                    path: '/games',
                    name: RouteName.Games,
                    component: () => import('~/pages/game/games-page.vue'),
                },
                {
                    path: '/lobby',
                    name: RouteName.Lobby,
                    component: () => import('~/pages/game/lobby-page.vue'),
                },
            ]
        }
    ]
});

router.beforeEach(routerAuthGuard);

export { router };
