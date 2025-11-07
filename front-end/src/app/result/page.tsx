"use client"

import { useRouter } from "next/navigation"
import { Button } from "@/components/ui/button"
import { CheckCircle2 } from "lucide-react"
import { useResultQuery } from "@/queries/use-result-query"
import { VotingResults } from "@/components/voting/voting-results"

export default function ResultPage() {
  const router = useRouter()
  const { data: result, isLoading } = useResultQuery()

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="max-w-6xl mx-auto px-4 py-8 md:py-12">
        {/* Success Message */}
        <div className="text-center mb-8 md:mb-12">
          <div className="flex justify-center mb-4">
            <CheckCircle2 className="w-12 h-12 md:w-16 md:h-16 text-green-600" />
          </div>
          <h1 className="text-xl md:text-2xl font-bold text-gray-900 mb-2">Seu voto foi registrado com sucesso</h1>
        </div>

        {/* Results Section */}
        {isLoading ? (
          <div className="text-center py-12">
            <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
            <p className="mt-4 text-gray-600 text-lg">Carregando resultados...</p>
          </div>
        ) : result ? (
          <VotingResults result={result} />
        ) : (
          <div className="text-center py-12 text-gray-600 text-lg">Não foi possível carregar os resultados</div>
        )}

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row justify-center gap-4 mt-8 md:mt-12">
          <Button
            size="lg"
            onClick={() => router.push("/")}
            className="bg-gray-900 text-white hover:bg-gray-800 text-base md:text-lg px-8 py-6 rounded-md font-semibold shadow-md transition-all hover:shadow-lg cursor-pointer"
          >
            VOTAR NOVAMENTE
          </Button>
        </div>
      </div>
    </main>
  )
}
