// import { Test, TestingModule } from '@nestjs/testing';
// import { INestApplication } from '@nestjs/common';
// import * as request from 'supertest';
// import { AppModule } from './../src/app.module';

// // import {CreatePersonDTO} from '../src/person/schemas/person.schema'

// describe('theAppController (e2e)', () => {
//   let app: INestApplication;

//   beforeEach(async () => {
//     const moduleFixture: TestingModule = await Test.createTestingModule({
//       imports: [AppModule],
//     }).compile();

//     app = moduleFixture.createNestApplication();
//     await app.init();
//   });

//   afterAll(async done => {
//     app.close();
//     done();
//   });

//   it('/ (GET)', async () => {
//     return request(app.getHttpServer())
//       .get('/')
//       .expect(200)
//       .expect('Hello World!');
//   });

//   // it('/ (GET)', () => {
//   //   return request(app.getHttpServer())
//   //     .get('person/getAll')
//   //     .expect('Elijah');
//   // });¡



//   // Make a body
//   it('/ (POST)', async done => {
//     request(app.getHttpServer())
//       .post('/person/add')
//       .send('lastName=Silly')
//       .expect(200);

//       done();
//   });

// });
