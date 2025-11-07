import { baseApiInstance } from "@/lib/axios";
import { BackendApiService } from "./backend-api-service";
import { StatsDto } from "./dtos/stats-dto";


export class VotingService extends BackendApiService {
    private readonly baseEndpoint = 'votes';

    async postVote(participantId: string): Promise<{ message: string }> {
        const response = await this.api.post<{ message: string }>(
            `${this.baseEndpoint}`,
            null,
            { params: { participantId } }
        );
        return response.data;
    }

    async getStats(): Promise<StatsDto> {
        const response = await this.api.get<StatsDto>(`${this.baseEndpoint}/stats`);
        return response.data;
    }

}

export const votingService = new VotingService(baseApiInstance);
