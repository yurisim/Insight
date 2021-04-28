import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { PersonService } from './person.service';
import { PersonController } from './person.controller';
import { PersonSchema } from './schemas/person.schema';

@Module({
  imports: [
    MongooseModule.forFeature([{ name: 'Person', schema: PersonSchema }]),
],
  providers: [PersonService],
  controllers: [PersonController]
})
export class PersonModule {}
