import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router'
import { useAuthStore } from '~/stores/auth-store'
import { RouteName } from '~/interfaces'

export async function routerAuthGuard(
  to: RouteLocationNormalized,
  _from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  const authStore = useAuthStore()
  const tokens = await authStore.getTokens()
  const isLoggedIn = !!tokens

  // 1) Гость идёт на /auth
  if (to.name === RouteName.Auth) {
    if (isLoggedIn) {
      // если уже залогинен — кидаем внутрь
      const redirect = authStore.afterAuthRedirectPath || '/'
      authStore.afterAuthRedirectPath = undefined
      return next({ path: redirect })
    }
    return next()
  }

  // 2) Защищённый маршрут
  if (to.meta.requiresAuth && !isLoggedIn) {
    // сохраняем, куда хотел попасть
    authStore.afterAuthRedirectPath = to.fullPath
    return next({ name: RouteName.Auth })
  }

  // 3) Всё прочее пропускаем
  return next()
}
