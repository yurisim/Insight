import { Test, TestingModule } from '@nestjs/testing';
import { INestApplication } from '@nestjs/common';
import * as request from 'supertest';
import { AppModule } from './../src/app.module';

import {CreatePersonDTO} from '../src/person/schemas/person.schema'


describe('theAppController (e2e)', () => {
  let app: INestApplication;

  beforeAll(async () => {
    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [AppModule],
    }).compile();

    app = moduleFixture.createNestApplication();
    await app.init();
  });

  afterAll( async () => {
    await app.close();
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

  // Make a body
  it('should be able to add a person', () => {

    const postData : CreatePersonDTO = {
      lastName: "ddfgdfg",
      firstName: "ddfgdfg",
      dodid: 1232141243,
      afscid: 124214,
      workCenter: "ddfgdfg",
      status: "Safe",
    }

    return request(app.getHttpServer())
      .post('/person/add')
      .send(postData)
      .expect(200);
  });

  it('should be able to find a specific ID', () => {

    return request(app.getHttpServer())
      .get('/person/get/60b7c192a5769d28841f67c8')
      .then(response => {
        expect(response.body.lastName).toBe('Silly')
      })
  });

  it('should not be able to find an ID', () => {

    return request(app.getHttpServer())
      .get('/person/get/676dsf78sydf786sdf6sd8f7f')
      .expect(400)
  });

  it('should be able to get all', () => {

    return request(app.getHttpServer())
      .get('/person/getAll')
      .then(res => {
        expect(Array.isArray(res.body)).toBeTruthy();
        expect(res.body.length).toBeGreaterThanOrEqual(1);
      })

  });
});
