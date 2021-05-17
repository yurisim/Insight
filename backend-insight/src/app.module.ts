import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';

import { MongooseModule } from '@nestjs/mongoose';
import { PersonModule } from './person/person.module';
import { TbaModule } from './tba/tba.module';
import { AfscModule } from './afsc/afsc.module';

@Module({
  imports: [
    // Connection string for mongoDB. This is the default port
    MongooseModule.forRoot('mongodb://localhost/27017', {
      useNewUrlParser: true,
    }),
    PersonModule,
    TbaModule,
    AfscModule,
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
