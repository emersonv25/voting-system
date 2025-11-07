import { Card } from "@/components/ui/card"
import { Progress } from "@/components/ui/progress"
import type { ResultDto } from "@/services/dtos/result-dto"

interface VotingResultsProps {
  result: ResultDto
}

export function VotingResults({ result }: VotingResultsProps) {
  if (!result || !result.participants || result.participants.length === 0) {
    return (
      <Card className="bg-white p-6 md:p-8 shadow-sm border border-gray-200">
        <h2 className="text-2xl md:text-3xl font-bold text-gray-900 mb-4 text-center">Resultado Parcial</h2>
        <p className="text-center text-gray-600">Nenhum voto registrado ainda. Seja o primeiro a votar!</p>
      </Card>
    )
  }

  // Sort participants by votes descending
  const sortedParticipants = [...result.participants].sort((a, b) => b.votes - a.votes)

  return (
    <div className="space-y-6">
      {/* Total Votes Card */}
      <Card className="bg-white p-6 md:p-8 shadow-sm border border-gray-200">
        <div className="text-center">
          <p className="text-sm md:text-base text-gray-600 uppercase tracking-wider mb-2">Total de votos</p>
          <p className="text-4xl md:text-5xl font-bold text-gray-900">{result.totalVotes.toLocaleString("pt-BR")}</p>
        </div>
      </Card>

      {/* Results Title */}
      <div className="text-center">
        <h2 className="text-2xl md:text-3xl font-bold text-gray-900">Resultado Parcial da Votação</h2>
      </div>

      {/* Participant Results - Similar to voting cards */}
      <div className="space-y-4">
        {sortedParticipants.map((participant, index) => (
          <Card
            key={participant.id}
            className="bg-white hover:shadow-md transition-shadow duration-200 border border-gray-200 overflow-hidden"
          >
            <div className="p-4 md:p-6">
              <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                {/* Participant Info */}
                <div className="flex-1">
                  <div className="flex items-center gap-3 mb-3">
                    <span className="text-2xl md:text-3xl font-bold text-gray-400">#{index + 1}</span>
                    <h3 className="text-xl md:text-2xl font-bold text-gray-900">{participant.name}</h3>
                  </div>

                  {/* Progress Bar */}
                  <div className="space-y-2">
                    <div className="flex justify-between items-center">
                      <span className="text-sm text-gray-600">{participant.votes.toLocaleString("pt-BR")} votos</span>
                      <span className="text-2xl md:text-3xl font-bold text-gray-900">
                        {participant.percentageOfVotes.toFixed(1)}%
                      </span>
                    </div>
                    <Progress value={participant.percentageOfVotes} className="h-3 md:h-4" />
                  </div>
                </div>

                {/* Participant Image */}
                <div className="flex justify-center md:justify-end">
                  <div className="relative w-24 h-24 md:w-32 md:h-32 rounded-full overflow-hidden border-4 border-gray-100">
                    <img
                      src={participant.photoUrl || "/placeholder.svg?height=128&width=128"}
                      alt={participant.name}
                      className="w-full h-full object-cover"
                    />
                  </div>
                </div>
              </div>
            </div>
          </Card>
        ))}
      </div>
    </div>
  )
}
