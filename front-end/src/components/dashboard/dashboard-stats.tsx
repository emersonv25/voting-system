import { Card } from "@/components/ui/card"
import { ResultDto } from "@/services/dtos/result-dto"
import { Users, TrendingUp, Clock } from "lucide-react"

interface DashboardStatsProps {
    result: ResultDto
}

export function DashboardStats({ result }: DashboardStatsProps) {
    const leader = result.participants.reduce((prev, current) => (prev.votes > current.votes ? prev : current))

    const leaderPercentage = result.totalVotes > 0 ? (leader.votes / result.totalVotes) * 100 : 0

    return (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <Card className="p-6 bg-white border border-neutral-200 shadow-sm hover:shadow-md transition-shadow">
                <div className="flex items-center gap-4">
                    <div className="p-3 bg-yellow-100 rounded-full">
                        <TrendingUp className="w-6 h-6 text-yellow-600" />
                    </div>
                    <div>
                        <p className="text-sm text-neutral-600 font-medium">Participante Líder</p>
                        {result.totalVotes > 0 ? (
                            <>
                                <p className="text-xl font-bold text-neutral-900">{leader.name}</p>
                                <p className="text-sm text-neutral-600">{leaderPercentage.toFixed(1)}% dos votos</p>
                            </>
                        ) : (
                            <p className="text-xl font-bold text-neutral-500">Nenhum voto ainda</p>
                        )}
                    </div>
                </div>
            </Card>
            <Card className="p-6 bg-white border border-neutral-200 shadow-sm hover:shadow-md transition-shadow">
                <div className="flex items-center gap-4">
                    <div className="p-3 bg-orange-100 rounded-full">
                        <Users className="w-6 h-6 text-orange-600" />
                    </div>
                    <div>
                        <p className="text-sm text-neutral-600 font-medium">Total de Votos</p>
                        <p className="text-3xl font-bold text-neutral-900">{result.totalVotes.toLocaleString("pt-BR")}</p>
                    </div>
                </div>
            </Card>
            <Card className="p-6 bg-white border border-neutral-200 shadow-sm hover:shadow-md transition-shadow">
                <div className="flex items-center gap-4">
                    <div className="p-3 bg-neutral-100 rounded-full">
                        <Clock className="w-6 h-6 text-neutral-600" />
                    </div>
                    <div>
                        <p className="text-sm text-neutral-600 font-medium">Última atualização</p>
                        <p className="text-xl font-bold text-neutral-900">
                            {new Date().toLocaleTimeString("pt-BR", {
                                hour: "2-digit",
                                minute: "2-digit",
                            })}
                        </p>
                    </div>
                </div>
            </Card>
        </div>
    )
}
