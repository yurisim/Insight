import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
// import { PersonModule } from './person/person.module';

import { MongooseModule } from '@nestjs/mongoose';

@Module({
  imports: [
    // Connection string for mongoDB. This is the default port
    MongooseModule.forRoot('mongodb://localhost/27017', {
      useNewUrlParser: true,
    }),
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
