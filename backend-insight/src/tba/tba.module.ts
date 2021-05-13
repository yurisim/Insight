import { Module } from '@nestjs/common';
import { TbaService } from './tba.service';
import { IController } from './i/i.controller';

@Module({
  providers: [TbaService],
  controllers: [IController]
})
export class TbaModule {}
