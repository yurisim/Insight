import { Test, TestingModule } from '@nestjs/testing';
import { TbaService } from './tba.service';

describe('TbaService', () => {
  let service: TbaService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [TbaService],
    }).compile();

    service = module.get<TbaService>(TbaService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });
});
