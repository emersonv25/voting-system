import { baseApiInstance } from "@/lib/axios";
import { BackendApiService } from "./backend-api-service";
import { ResultDto } from "./dtos/result-dto";

export class ResultService extends BackendApiService {
    private readonly baseEndpoint = 'results';
    async getResult(): Promise<ResultDto> {
        const response = await this.get<ResultDto>(`${this.baseEndpoint}`);
        console.log('Fetched result:', response.data);
        return response.data!;
    }

}

export const resultService = new ResultService(baseApiInstance);