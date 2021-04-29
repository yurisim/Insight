## Installation

```bash
$ npm install
```


## Running the app

You MUST have MongoDB installed and running. Verify that it is running on local host 27017. (ensure that mongod.exe has a working data directory). For windows machines,



```bash
# make the mongo directory
$ md "\data\db"

#then navigate to your mongo installation drive and start mongod.exe on the cmd prompt

# development
$ npm run start

# watch mode
$ npm run start:dev

# production mode
$ npm run start:prod
```

## Test

```bash
# unit tests
$ npm run test

# e2e tests
$ npm run test:e2e

# test coverage
$ npm run test:cov
```

## Support

Nest is an MIT-licensed open source project. It can grow thanks to the sponsors and support by the amazing backers. If you'd like to join them, please [read more here](https://docs.nestjs.com/support).

## Stay in touch

- Author - [Kamil Myśliwiec](https://kamilmysliwiec.com)
- Website - [https://nestjs.com](https://nestjs.com/)
- Twitter - [@nestframework](https://twitter.com/nestframework)

## License

Nest is [MIT licensed](LICENSE).
