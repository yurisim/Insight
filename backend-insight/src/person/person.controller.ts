import { Controller, Get, Res, HttpStatus, Param, NotFoundException, Post, Body, Put, Query, Delete } from '@nestjs/common';
//import { CreatePersonDto, UpdatePersonDto, ListAllEntities } from './dto';
import { PersonService } from './person.service';
import { CreatePersonDTO } from './dto/create-person.dto';
import { ValidateObjectId } from 'src/shared/validate-object-id';

@Controller('person')
export class PersonController {

  // Be consistent w/ plurals and singulars
  constructor(private personService: PersonService) {}

  // @Post()
  // createPerson(@Body() CreatePersonDTO: CreatePersonDTO) {
  //   return this.personService.addPerson(CreatePersonDTO);
  // }

  @Post('/person')
  async addPerson(@Res() res, @Body() createPostDTO: CreatePersonDTO) {
    const addedPerson = await this.personService.addPerson(createPostDTO);
    return res.status(HttpStatus.OK).json({
      message: 'Post has been submitted successfully!',
      post: addedPerson,
    });
  }

 /* @Get()
  findAll(@Query() query: ListAllEntities) {
    return `This action returns all persons (limit: ${query.limit} items)`;
  }*/

  // @Get(':id')
  // findOne(@Param('id') id: string) {
  //   return this.personService;
  // }

  @Get('person/:personID')
  async getPost(@Res() res, @Param('personID', new ValidateObjectId()) personID) {
    const person = await this.personService.getPerson(personID);

    if (!person) {
        throw new NotFoundException('Post does not exist!');
    }

    return res.status(HttpStatus.OK).json(person);
  }

  @Get('persons')
  async getAllPersons(@Res() res) {
    const persons = await this.personService.getAllPersons();
    return res.status(HttpStatus.OK).json(persons);
  }

  /*@Put(':id')
  update(@Param('id') id: string, @Body() updatePersonDto: UpdatePersonDto) {
    return `This action updates a #${id} person`;
  }*/

  @Put('person/edit')
  async editPost(
    @Res() res,
    @Query('personID', new ValidateObjectId()) personID,
    @Body() createPersonDTO: CreatePersonDTO,
  ) {
    const editedPerson = await this.personService.editPerson(personID, createPersonDTO);
    if (!editedPerson) {
        throw new NotFoundException('Person does not exist!');
    }
    return res.status(HttpStatus.OK).json({
      message: 'Person has been successfully updated',
      post: editedPerson,
    });
  }

  // @Delete(':id')
  // remove(@Param('id') id: string) {
  //   return `This action removes a #${id} person`;
  // }

  @Delete('person/delete')
  async deletePerson(@Res() res, @Query('personID', new ValidateObjectId()) personID) {
    const deletedPerson = await this.personService.deletePerson(personID);

    if (!deletedPerson) {
        throw new NotFoundException('Person does not exist!');
    }

    return res.status(HttpStatus.OK).json({
      message: 'Person has been deleted!',
      post: deletedPerson,
    });
  }
}