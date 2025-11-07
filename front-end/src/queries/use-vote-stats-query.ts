import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { votingService } from '@/services/voting.service';
import { AxiosError } from 'axios';
import { ApiErrorResponse } from '@/types/api-response';
import { toast } from 'sonner';

export function useVotingStatsQuery() {
    return useQuery({
        queryKey: ['voting-stats'],
        queryFn: () => votingService.getStats(),
    });
}

export function useVoteMutation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (participantId: string) => votingService.postVote(participantId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['voting-stats'] });
            queryClient.invalidateQueries({ queryKey: ['result'] });
        },
        onError: (error: AxiosError<ApiErrorResponse>) => {
            const errorMessage =
                error.response?.data?.message || 'Erro ao registrar voto';
            toast.error('Erro ao registrar voto', {
                description: errorMessage,
            });
        },
    });
}
