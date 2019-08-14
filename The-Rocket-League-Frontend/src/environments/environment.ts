// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  baseUrl: 'http://localhost:5000/api',
  testToken: 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9' +
    '.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJqb2huIiwibmJmIjoxNTY1ODEzMzkzLCJleHAiOjE1NjU5ODYxOTMsImlhdCI6MTU2NTgxMzM5M30' +
    '.6rBG3R02n63P0MJIwGjieuLbLhsRO5MHx7r6pRjrAjwFavjJYoFHvMJGLrv-auZ5sQjUjv_A0IP4dYaJ8_7-Xg',
  authTokenKey: 'authTokenKey'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
