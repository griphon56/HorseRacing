import { NavigationGuard } from "vue-router";
import { RouteName } from '~/interfaces';
import { useAuthStore } from "./auth-store";

export const routerAuthGuard: NavigationGuard = async (to) => {
    const authStore = useAuthStore();

    if (to.name == RouteName.Auth) { // Переход на страницу авторизации всегда разрешен
        return;
    }

    if (authStore.user) { // Пользователь авторизован и просто переходит на другую страницу, пускаем
        return;
    }

    const receivedUser = await authStore.loadUserOrTryLogIn();
    if (receivedUser == null) {
        authStore.afterAuthRedirectPath = to.fullPath;
        return { name: RouteName.Auth };
    }
};
