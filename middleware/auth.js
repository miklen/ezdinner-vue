export default async function ({ app, redirect }) {
  // If the user is not authenticated
  console.log('checking auth')
  const isAuthd = await app.$msal.getIsAuthenticated()
  if (!isAuthd) {
    return redirect('/')
  }
}
