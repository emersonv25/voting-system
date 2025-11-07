import { Card } from "@/components/ui/card"
import { StatsDto } from "@/services/dtos/stats-dto"

interface VotesPerHourChartProps {
  stats: StatsDto
}

export function VotesPerHourChart({ stats }: VotesPerHourChartProps) {
  const data = stats.votesPerHour
  const maxVotes = Math.max(...data.map((item) => item.total))

  return (
    <Card className="p-6 bg-white border border-neutral-200 shadow-sm">
      <h2 className="text-2xl font-bold text-neutral-900 mb-6">Votos por Hora</h2>

      <div className="space-y-4">
        {data.map((item) => {
          const percentage = maxVotes > 0 ? (item.total / maxVotes) * 100 : 0
          const hourString = new Date(item.hour).toLocaleTimeString("pt-BR", {
            hour: "2-digit",
            minute: "2-digit",
          })

          return (
            <div key={item.hour.toString()} className="flex items-center gap-4">
              <span className="text-sm font-semibold text-neutral-700 w-16 text-right">{hourString}</span>
              <div className="flex-1 relative">
                <div className="bg-neutral-200 h-8 rounded-full overflow-hidden">
                  <div
                    className="bg-gradient-to-r from-orange-500 to-yellow-500 h-full flex items-center justify-end pr-3 transition-all duration-500"
                    style={{ width: `${percentage}%` }}
                  >
                    {percentage > 15 && (
                      <span className="text-sm font-semibold text-white">{item.total.toLocaleString("pt-BR")}</span>
                    )}
                  </div>
                </div>
              </div>
              {percentage <= 15 && (
                <span className="text-sm font-semibold text-neutral-700 w-20 text-left">
                  {item.total.toLocaleString("pt-BR")}
                </span>
              )}
            </div>
          )
        })}
      </div>
    </Card>
  )
}
