"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { toast } from "sonner"
import { useVoteMutation } from "@/queries/use-vote-mutation"
import { useResultQuery } from "@/queries/use-result-query"
import { Button } from "@/components/ui/button"
import { VotingCard } from "@/components/voting-card"
import { Spinner } from "@/components/ui/spinner"
import { Eye } from "lucide-react"

export default function Home() {
  const router = useRouter()
  const [selectedParticipant, setSelectedParticipant] = useState<string | null>(null)
  const voteMutation = useVoteMutation()
  const { data: result, isLoading } = useResultQuery()

  const handleVote = async () => {
    if (!selectedParticipant) {
      toast.error("Por favor, selecione um participante para votar")
      return
    }

    await voteMutation.mutateAsync(selectedParticipant)
    router.push("/result")
  }

  if (isLoading) {
    return (
      <div className="min-h-screen bg-white flex items-center justify-center">
        <Spinner className="size-12 text-zinc-700" />
      </div>
    )
  }

  if (!result || result.participants.length === 0) {
    return (
      <div className="min-h-screen bg-white flex items-center justify-center">
        <div className="text-center">
          <p className="text-zinc-900 text-2xl mb-4">Nenhum participante disponível</p>
          <Button onClick={() => window.location.reload()} className="bg-zinc-800 hover:bg-zinc-700 text-white">
            Recarregar
          </Button>
        </div>
      </div>
    )
  }

  const selectedParticipantData = result.participants.find((p) => p.id === selectedParticipant)

  return (
    <div className="min-h-screen bg-white">
      {/* Main Content */}
      <div className="max-w-4xl mx-auto px-4 py-6 md:py-12">
        <div className="mb-8 md:mb-12">
          <h1 className="text-2xl md:text-4xl lg:text-5xl font-bold text-zinc-900 text-balance">
            Paredão BBB25: Vote para eliminar. {result.participants.map((p) => p.name).join(", ")}?
          </h1>
        </div>

        <div className="space-y-4 md:space-y-6 mb-8">
          {result.participants.map((participant) => (
            <VotingCard
              key={participant.id}
              participant={{
                id: participant.id,
                name: participant.name,
                imageUrl: participant.photoUrl,
              }}
              isSelected={selectedParticipant === participant.id}
              onSelect={() => setSelectedParticipant(participant.id)}
            />
          ))}
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col items-center gap-4 mt-8">
          <Button
            size="lg"
            onClick={handleVote}
            disabled={!selectedParticipant || voteMutation.isPending}
            className="bg-orange-500 hover:bg-orange-600 text-white text-xl md:text-2xl px-12 md:px-16 py-6 md:py-8 rounded-lg font-bold shadow-lg transform transition hover:scale-105 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:scale-100 uppercase w-full md:w-auto cursor-pointer"
          >
            {voteMutation.isPending ? (
              <span className="flex items-center gap-3">
                <Spinner className="size-6" />
                Votando...
              </span>
            ) : (
              "Votar"
            )}
          </Button>

          <Button
            variant="ghost"
            onClick={() => router.push("/dashboard")}
            className="text-zinc-600 hover:text-zinc-900 hover:bg-zinc-100 text-sm md:text-base px-4 py-2 rounded flex items-center gap-2 cursor-pointer"
          >
            <Eye className="size-4" />
            Ver Resultados
          </Button>
        </div>
      </div>
    </div>
  )
}
