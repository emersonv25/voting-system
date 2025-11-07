import { ParticipantDto } from "./participant-dto";

export interface ResultDto {
    participants: ParticipantDto[];
    totalVotes: number;
}