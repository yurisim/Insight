import { Test, TestingModule } from '@nestjs/testing';
import { PersonController } from './person.controller';
import { PersonService } from './person.service';
import { Person } from './schemas/Person.schema';

describe('PersonController', () => {

  let personController: PersonController;
  let personService: PersonService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [PersonController],
      providers: [PersonService],
    }).compile();

    personService = module.get<PersonService>(PersonService);
    personController = module.get<PersonController>(PersonController);
  });

  afterEach(() => {
    jest.restoreAllMocks();
    jest.resetAllMocks();
  });

  // it('should be defined', () => {
  //   expect(personService).toBeDefined();
  //   expect(personService).toBeDefined();
  // });

  // describe('findAll', () => {
  //   it('should return an array of people', async () => {
  //     const result: Person[] = [];

  //     // Make the Cook Grab something premade from the fridge
  //     jest.spyOn(personService, 'getAllPersons').mockImplementation(async () => result);

  //     let res: any;

  //     expect(await personController.getAllPersons(res)).toBe(result);
  //   });
  // })
})