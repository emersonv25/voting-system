import { resultService } from "@/services/result.service";
import { useQuery } from "@tanstack/react-query";

export function useResultQuery() {
  return useQuery({
    queryKey: ['result'],
    queryFn: () => resultService.getResult(),
  });
}