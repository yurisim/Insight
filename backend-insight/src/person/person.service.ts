import { Injectable } from '@nestjs/common';
import { Model } from 'mongoose';
import { InjectModel } from '@nestjs/mongoose';
import { Person } from './schemas/Person.schema';
import { CreatePersonDTO } from './dto/create-person.dto';

@Injectable()
export class PersonService {
  //private Persons: Person[] = [];

  //constructor(@InjectModel(Person.name) private personModel: Person) {}

  // Need to use a PersonDocument in this because the Model requires a document
  constructor(
    @InjectModel('Person') private readonly personModel: Model<Person>,
  ) {}

  // Add Person into system
  async addPerson(createPersonDTO: CreatePersonDTO): Promise<Person> {
    const addedPerson = new this.personModel(createPersonDTO);

    return addedPerson.save();
  }

  async getPerson(personID: any): Promise<Person> {
    // add await in case this takes a hot minute
    const foundPerson = await this.personModel.findById(personID).exec();
    return foundPerson;
  }

  async getAllPersons(): Promise<Person[]> {
    // add await in case this takes a hot minute
    const allPersons = await this.personModel.find().exec();
    return allPersons;
  }

  async editPerson(personID: any, createPersonDTO: CreatePersonDTO): Promise<Person> {
    const editedPerson = await this.personModel.findByIdAndUpdate(
      personID,
      createPersonDTO,
      { new: true },
    );
    return editedPerson;
  }

  async deletePerson(personID: any): Promise<any> {
    const deletedPerson = await this.personModel.findByIdAndRemove(personID);
    return deletedPerson;
  }
}
