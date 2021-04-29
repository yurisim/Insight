import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';

import { MongooseModule } from '@nestjs/mongoose';
import { PersonModule } from './person/person.module';

@Module({
  imports: [
    // Connection string for mongoDB. This is the default port
    MongooseModule.forRoot('mongodb://localhost/27017', {
      useNewUrlParser: true,
    }),
    PersonModule,
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
