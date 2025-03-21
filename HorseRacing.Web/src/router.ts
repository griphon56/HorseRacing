import { createRouter, createWebHistory } from 'vue-router';
import { MainLayout } from '~/components/layout';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/auth',
            name: 'auth',
            component: () => import('~/pages/auth-page.vue'),
        },
        {
            path: '/',
            component: MainLayout,
            children: [
                {
                    path: '/account',
                    name: 'account',
                    component: () => import('~/pages/account/profile-info.vue'),
                },
                {
                    path: '/games',
                    name: 'aaa',
                    component: () => import('~/pages/games-page.vue'),
                },
                {
                    path: '/play',
                    name: 'play',
                    component: () => import('~/pages/account/profile-info.vue'),
                },
            ]
        }
    ]
});



export { router };
