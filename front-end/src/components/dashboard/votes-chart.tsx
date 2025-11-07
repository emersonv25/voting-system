import { Card } from "@/components/ui/card"
import { ResultDto } from "@/services/dtos/result-dto"

interface VotesChartProps {
  result: ResultDto
}

export function VotesChart({ result }: VotesChartProps) {
  const hasVotes = result.totalVotes > 0

  return (
    <Card className="p-6 bg-white border border-neutral-200 shadow-sm">
      <h2 className="text-2xl font-bold text-neutral-900 mb-6">Distribuição de Votos por Participante</h2>

      {hasVotes ? (
        <div className="space-y-6">
          {result.participants.map((participant) => {
            const percentage = (participant.votes / result.totalVotes) * 100

            return (
              <div key={participant.id} className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-lg font-semibold text-neutral-800">{participant.name}</span>
                  <div className="text-right">
                    <span className="text-xl font-bold text-neutral-900">{percentage.toFixed(1)}%</span>
                    <p className="text-sm text-neutral-600">{participant.votes.toLocaleString("pt-BR")} votos</p>
                  </div>
                </div>
                {/* Barra de progresso personalizada */}
                <div className="w-full bg-neutral-200 rounded-full h-6 overflow-hidden">
                  <div
                    className="bg-gradient-to-r from-orange-500 to-yellow-500 h-full rounded-full transition-all duration-500 flex items-center justify-end px-3"
                    style={{ width: `${percentage}%` }}
                  >
                    {percentage > 10 && (
                      <span className="text-xs font-semibold text-white">{percentage.toFixed(1)}%</span>
                    )}
                  </div>
                </div>
              </div>
            )
          })}
        </div>
      ) : (
        <div className="text-center py-12">
          <p className="text-neutral-500 text-lg">Nenhum voto registrado ainda.</p>
          <p className="text-neutral-400 text-sm mt-2">
            Os dados aparecerão aqui assim que os votos começarem a ser contabilizados.
          </p>
        </div>
      )}
    </Card>
  )
}
