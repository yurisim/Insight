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
      .get('/person/get/60b7f69703ee000ce48ae958')
      .then(response => {
        expect(response.body.lastName).toBe('ddfgdfg')
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

  it('should be able to search for a specific Workcenter', () => {

    return request(app.getHttpServer())
      .get('/person/search/DOUP')
      .then(response => {
        expect(response.body[0].workCenter).toBe('DOUP')
      })
  });

  it('should be able to search for a specific name', () => {

    return request(app.getHttpServer())
      .get('/person/search/Grif Castle')
      .then(response => {
        expect(response.body[0].name).toBe('Grif Castle')
      })
  });

  // it('should delete person made by test', () => {

  //   return request(app.getHttpServer())
  //     .delete('/person/delete/1232141243')
  //     .then(response => {
  //       expect(response.body.message).toBe('Person has been deleted!')
  //     })
  // });

  it('should be able to edit a specific person based on ID', () => {

    return request(app.getHttpServer())
      .put('/person/edit/60b7f69703ee000ce48ae958')
      .then(response => {
        expect(response.body.message).toBe('Person has been successfully updated')
      })
  });

});
