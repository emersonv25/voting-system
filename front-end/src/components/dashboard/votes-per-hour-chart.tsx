import { Card } from "@/components/ui/card"
import { StatsDto } from "@/services/dtos/stats-dto"

interface VotesPerHour {
  hour: string | Date
  total: number
}

interface VotesPerHourChartProps {
  stats: StatsDto
}

export function VotesPerHourChart({ stats }: VotesPerHourChartProps) {
  const data: VotesPerHour[] = stats.votesPerHour ?? []

  const sortedData = [...data].sort(
    (a, b) => new Date(b.hour).getTime() - new Date(a.hour).getTime()
  )

  const groupedByDate = sortedData.reduce<Record<string, VotesPerHour[]>>((acc, item) => {
    const date = new Date(item.hour)
    const dateKey = date.toLocaleDateString("pt-BR", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    })
    if (!acc[dateKey]) acc[dateKey] = []
    acc[dateKey].push(item)
    return acc
  }, {})

  const sortedDates = Object.keys(groupedByDate).sort((a, b) => {
    const [da, ma, ya] = a.split("/").map(Number)
    const [db, mb, yb] = b.split("/").map(Number)
    return new Date(yb, mb - 1, db).getTime() - new Date(ya, ma - 1, da).getTime()
  })

  return (
    <Card className="p-6 bg-white border border-neutral-200 shadow-sm">
      <h2 className="text-2xl font-bold text-neutral-900 mb-6">Votos por Hora</h2>

      <div className="space-y-8">
        {sortedDates.map((date) => {
          const items = groupedByDate[date]

          const sortedItems = [...items].sort(
            (a, b) => new Date(b.hour).getTime() - new Date(a.hour).getTime()
          )

          const maxVotes = Math.max(...sortedItems.map((item) => item.total))

          return (
            <div key={date}>
              <h3 className="text-lg font-semibold text-neutral-800 mb-4">{date}</h3>

              <div className="space-y-4">
                {sortedItems.map((item) => {
                  const percentage = maxVotes > 0 ? (item.total / maxVotes) * 100 : 0
                  const hourString = new Date(item.hour).toLocaleTimeString("pt-BR", {
                    hour: "2-digit",
                    minute: "2-digit",
                  })

                  return (
                    <div key={item.hour.toString()} className="flex items-center gap-4">
                      <span className="text-sm font-semibold text-neutral-700 w-16 text-right">
                        {hourString}
                      </span>

                      <div className="flex-1 relative">
                        <div className="bg-neutral-200 h-8 rounded-full overflow-hidden">
                          <div
                            className="bg-linear-to-r from-orange-500 to-yellow-500 h-full flex items-center justify-end pr-3 transition-all duration-500"
                            style={{ width: `${percentage}%` }}
                          >
                            {percentage > 15 && (
                              <span className="text-sm font-semibold text-white">
                                {item.total.toLocaleString("pt-BR")}
                              </span>
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
            </div>
          )
        })}
      </div>
    </Card>
  )
}
