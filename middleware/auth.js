export default async function ({ app, redirect }) {
  // If the user is not authenticated
  const isAuthd = await app.$msal.getIsAuthenticated()
  if (!isAuthd) {
    return redirect('/')
  }
}
