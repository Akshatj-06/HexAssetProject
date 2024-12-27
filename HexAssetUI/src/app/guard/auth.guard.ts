import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';


export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  // Retrieve token from localStorage
  const token = localStorage.getItem('JwtToken');
  if (token) {
    try {
      // Decode the token to get the user role
      const decodedToken: any = jwtDecode(token);
      const userRole = decodedToken.role; // Assumes 'role' is a property in the JWT payload.

      // Retrieve required role from route data
      const requiredRole = route.data['role'] as string;

      if (requiredRole && userRole === requiredRole) {
        return true;
      } else {
        // Redirect to unauthorized page if role mismatch
        router.navigateByUrl('/unauthorized');
        return false;
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      router.navigateByUrl('/login');
      return false;
    }
  } else {
    // Redirect to login if no token found
    router.navigateByUrl('/login');
    return false;
  }
};
