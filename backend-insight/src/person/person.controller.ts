import { Controller, Get, Res, HttpStatus, Param, NotFoundException, Post, Body, Put, Query, Delete } from '@nestjs/common';
//import { CreatePersonDto, UpdatePersonDto, ListAllEntities } from './dto';
import { PersonService } from './person.service';
import { CreatePersonDTO } from './dto/create-person.dto';
import { ValidateDoDID } from 'src/shared/validate-object-id';

@Controller('person')
export class PersonController {

  // Be consistent w/ plurals and singulars
  constructor(private personService: PersonService) {}

  // @Post()
  // createPerson(@Body() CreatePersonDTO: CreatePersonDTO) {
  //   return this.personService.addPerson(CreatePersonDTO);
  // }
  
  //Is part of the URL when adding a new person
  @Post('add')
  async addPerson(@Res() res, @Body() addPersonDTO: CreatePersonDTO) {
    const addedPerson = await this.personService.addPerson(addPersonDTO);
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

  // Get Person of a specific ID
  @Get('get/:DoDID')
  async getPost(@Res() res, @Param('DoDID', new ValidateDoDID()) DoDID) {
    const person = await this.personService.getPerson(DoDID);

    if (!person) {
        throw new NotFoundException('Post does not exist!');
    }

    return res.status(HttpStatus.OK).json(person);
  }

  // Get Everyone
  @Get('getAll')
  async getAllPersons(@Res() res) {
    const persons = await this.personService.getAllPersons();
    return res.status(HttpStatus.OK).json(persons);
  }

  /*@Put(':id')
  update(@Param('id') id: string, @Body() updatePersonDto: UpdatePersonDto) {
    return `This action updates a #${id} person`;
  }*/

  @Put('edit')
  async editPost(
    @Res() res,
    @Query('DoDID', new ValidateDoDID()) DoDID,
    @Body() createPersonDTO: CreatePersonDTO,
  ) {
    const editedPerson = await this.personService.editPerson(DoDID, createPersonDTO);
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

  @Delete('delete')
  async deletePerson(@Res() res, @Query('DoDID', new ValidateDoDID()) DoDID) {
    const deletedPerson = await this.personService.deletePerson(DoDID);

    if (!deletedPerson) {
        throw new NotFoundException('Person does not exist!');
    }

    return res.status(HttpStatus.OK).json({
      message: 'Person has been deleted!',
      post: deletedPerson,
    });
  }
}