import { Controller, Get, Query, Post, Body, Put, Param, Delete } from '@nestjs/common';
//import { CreatePersonDto, UpdatePersonDto, ListAllEntities } from './dto';
import { PersonsService } from './person.service';
import { Person } from './schemas/person.schema'
import { CreatePersonDto } from './dto/create-person.dto';

@Controller('person')
export class PersonController {

  constructor(private personsService: PersonsService) {}

  @Post()
  createPerson(@Body() createPersonDto: CreatePersonDto) {
    return this.personsService.createPerson(CreatePersonDto);
  }

 /* @Get()
  findAll(@Query() query: ListAllEntities) {
    return `This action returns all persons (limit: ${query.limit} items)`;
  }*/

  @Get(':id')
  findOne(@Param('id') id: string) {
    return this.personsService
  }

  /*@Put(':id')
  update(@Param('id') id: string, @Body() updatePersonDto: UpdatePersonDto) {
    return `This action updates a #${id} person`;
  }*/

  @Delete(':id')
  remove(@Param('id') id: string) {
    return `This action removes a #${id} person`;
  }
}