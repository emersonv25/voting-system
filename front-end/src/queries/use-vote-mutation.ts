import { votingService } from "@/services/voting.service";
import { ApiErrorResponse } from "@/types/api-response";
import { useQueryClient, useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import { toast } from "sonner";

export function useVoteMutation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (participantId: string) => votingService.postVote(participantId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['voting-stats'] });
            queryClient.invalidateQueries({ queryKey: ['result'] });
            toast.success('Voto registrado com sucesso!');

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
