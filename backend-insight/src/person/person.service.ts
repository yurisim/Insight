import { Model } from 'mongoose';
import { Injectable } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Person, PersonDocument } from './schemas/Person.schema';
import { CreatePersonDto } from './dto/create-person.dto';

@Injectable()
export class PersonsService {
  private Persons : Person[] = [];


  constructor(@InjectModel(Person.name) private personModel: Person) {}

  async createPerson(createPersonDto: CreatePersonDto): Promise<Person> {
    const createdPerson = new this.personModel(createPersonDto);
    return createdPerson.save();
  }

  async findAll(): Promise<Person[]> {
    return this.personModel.find().exec();
  }
}
