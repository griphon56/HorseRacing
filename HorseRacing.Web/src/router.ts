import { createRouter, createWebHistory } from 'vue-router';
import { routerAuthGuard } from '~/core/auth';
import { RouteName } from '~/interfaces';
import { MainLayout } from '~/components/layout';

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/auth',
            name: RouteName.Auth,
            component: () => import('~/pages/auth/auth-page.vue'),
            meta: { guest: true },
        },
        {
            path: '/register',
            name: RouteName.Register,
            component: () => import('~/pages/auth/register-page.vue'),
            meta: { guest: true },
        },
        {
            path: '/',
            redirect: '/games',
            component: MainLayout,
            children: [
                {
                    path: 'profile',
                    name: RouteName.Profile,
                    component: () => import('~/pages/user/profile-page.vue'),
                    meta: { requiresAuth: true },
                },
                {
                    path: 'games',
                    name: RouteName.Games,
                    component: () => import('~/pages/game/games-page.vue'),
                    meta: { requiresAuth: true },
                },
                {
                    path: '/games/lobby/:id',
                    name: RouteName.Lobby,
                    component: () => import('~/pages/game/lobby-page.vue'),
                    meta: { requiresAuth: true },
                    props: true,
                },
                {
                    path: '/games/create',
                    name: RouteName.CreateRoom,
                    component: () => import('~/pages/game/create-page.vue'),
                    meta: { requiresAuth: true },
                },
                {
                    path: '/games/join/:id',
                    name: RouteName.JoinGame,
                    component: () => import('~/pages/game/join-page.vue'),
                    meta: { requiresAuth: true },
                    props: true,
                },
                {
                    path: '/games/race/:id',
                    name: RouteName.Race,
                    component: () => import('~/pages/game/race-page.vue'),
                    meta: { requiresAuth: true },
                    props: true,
                },
                {
                    path: '/games/result/:id',
                    name: RouteName.Result,
                    component: () => import('~/pages/game/result-page.vue'),
                    meta: { requiresAuth: true },
                    props: true,
                },
            ],
        },
        { path: '/:pathMatch(.*)*', redirect: '/games' },
    ],
});

router.beforeEach(routerAuthGuard);
