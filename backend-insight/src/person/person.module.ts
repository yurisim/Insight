import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { PersonsService } from './person.service';
import { PersonController } from './person.controller';
import { Person, PersonSchema } from './schemas/person.schema';

@Module({
  imports: [MongooseModule.forFeature([{ name : Person.name, schema : PersonSchema }])],
  providers: [PersonsService],
  controllers: [PersonController]
})
export class PersonModule {}
