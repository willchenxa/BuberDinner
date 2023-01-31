# Buber dinner API

- [Buber Dinner API](#buber-dinner-api)
  - [Auth](#auth)
   - [Register](#register)
     - [Register Request](#register-request)
     - [Register Response](#register-response)
   - [Login](#login)
     - [Login Request](#login-request)
     - [Login Response](#login-request)

## Auth

### Register


```js
POST {{host}}/auth/register
```

### Register Request

```json
{
 "firstName": "William",
 "lastName": "Chen",
 "email": "will.chenxa@gmail.com",
 "password": "12345678"
}
```