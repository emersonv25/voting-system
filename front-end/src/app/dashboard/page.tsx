"use client"

import { useRouter } from "next/navigation"
import { Button } from "@/components/ui/button"
import { ArrowLeft } from "lucide-react"
import { DashboardStats } from "@/components/dashboard/dashboard-stats"
import { VotesChart } from "@/components/dashboard/votes-chart"
import { VotesPerHourChart } from "@/components/dashboard/votes-per-hour-chart"
import { useResultQuery } from "@/queries/use-result-query"
import { useVotingStatsQuery } from "@/queries/use-vote-stats-query"

export default function DashboardPage() {
  const router = useRouter()
  const { data: result, isLoading: isLoadingResult } = useResultQuery()
  const { data: stats, isLoading: isLoadingStats } = useVotingStatsQuery();

  const isLoading = isLoadingResult || isLoadingStats

  return (
  <div className="h-full bg-neutral-50 flex flex-col">
      {/* Header da página do dashboard */}
  <div className="bg-neutral-800 text-white shadow-md shrink-0">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center gap-4">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => router.push("/")}
              className="text-white hover:bg-white/10"
            >
              <ArrowLeft className="w-5 h-5" />
            </Button>
            <h1 className="text-xl md:text-2xl font-bold">Dashboard - Estatísticas de Votação</h1>
          </div>
        </div>
      </div>

      {/* Conteúdo principal */}
  <div className="container mx-auto px-4 py-8 flex-1 flex flex-col justify-center">
        {isLoading ? (
          <div className="text-center text-neutral-600 text-lg py-12">Carregando estatísticas...</div>
        ) : result && stats ? (
          <div className="space-y-6">
            <DashboardStats result={result} />
            <VotesChart result={result} />
            {stats.votesPerHour && stats.votesPerHour.length > 0 && <VotesPerHourChart stats={stats} />}
          </div>
        ) : (
          <div className="text-center text-neutral-600 text-lg py-12">Não foi possível carregar as estatísticas</div>
        )}
      </div>
    </div>
  )
}
