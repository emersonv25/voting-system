import { VotesPerHourDto } from "./votes-per-hour-dto";

export interface StatsDto {
    totalVotes: number;
    votesPerHour: VotesPerHourDto[];
}