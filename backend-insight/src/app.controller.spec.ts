// import { Test, TestingModule } from '@nestjs/testing';
// import { AppController } from './app.controller';
// import { AppService } from './app.service';

// describe('AppController', () => {
//   let appController: AppController;

//   beforeEach(async () => {
//     const app: TestingModule = await Test.createTestingModule({
//       controllers: [AppController],
//       providers: [AppService],
//     }).compile();

//     appController = app.get<AppController>(AppController);
//   });

//   describe('root', () => {
//     it('should return "Hello World!"', () => {
//       expect(appController.getHello()).toBe('Hello World!');
//     });
//   });
// });


// describe('Basic', () => {
//   describe('the basic test', () => {
//     it('should return "true!"', () => {
//       expect(true).toBeTruthy();
//     });
//   });
// });

import { Test, TestingModule } from '@nestjs/testing';
import { INestApplication } from '@nestjs/common';
import * as request from 'supertest';
import { AppModule } from './../src/app.module';

describe('theAppController (e2e)', () => {
  let app: INestApplication;

  beforeEach(async () => {
    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [AppModule],
    }).compile();

    app = moduleFixture.createNestApplication();
    await app.init();
  });

  afterAll( done => {
    app.close();
    done();
  });

  it('/ (GET)', () => {
    return request(app.getHttpServer())
      .get('/')
      .expect(200)
      .expect('Hello World!');
  });

  // it('/ (GET)', () => {
  //   return request(app.getHttpServer())
  //     .get('person/getAll')
  //     .expect('Elijah');
  // });¡



  // // Make a body
  // it('/ (POST)', async done => {
  //   request(app.getHttpServer())
  //     .post('/person/add')
  //     .send('lastName=Silly')
  //     .expect(200);

  //     done();
  // });

});
