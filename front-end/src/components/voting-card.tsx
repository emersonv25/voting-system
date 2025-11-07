"use client"

import { cn } from "@/lib/utils"
import { Card } from "./ui/card"

interface VotingCardProps {
  participant: {
    id: string
    name: string
    imageUrl: string
  }
  isSelected: boolean
  onSelect: () => void
}

export function VotingCard({ participant, isSelected, onSelect }: VotingCardProps) {
  return (
    <Card
      onClick={onSelect}
      className={cn(
        "relative cursor-pointer transition-all duration-200 overflow-hidden bg-white border hover:shadow-lg",
        isSelected ? "ring-2 ring-orange-500 border-orange-500 shadow-lg" : "border-zinc-300 hover:border-zinc-400",
      )}
    >
      <div className="flex items-center justify-between p-4 md:p-6 min-h-[120px] md:min-h-[140px]">
        <div className="flex-1">
          <h3 className="text-2xl md:text-3xl lg:text-4xl font-bold text-zinc-800">{participant.name}</h3>
        </div>

        <div className="relative w-24 h-24 md:w-32 md:h-32 lg:w-36 lg:h-36 flex-shrink-0 ml-4">
          {participant.imageUrl ? (
            <img
              src={participant.imageUrl || "/placeholder.svg"}
              alt={participant.name}
              className="w-full h-full object-cover rounded"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center bg-zinc-200 rounded">
              <span className="text-3xl md:text-4xl font-bold text-zinc-400">{participant.name.charAt(0)}</span>
            </div>
          )}

          {isSelected && (
            <div className="absolute inset-0 bg-orange-500/20 rounded flex items-center justify-center">
              <div className="w-8 h-8 md:w-10 md:h-10 rounded-full bg-orange-500 flex items-center justify-center">
                <svg
                  className="w-5 h-5 md:w-6 md:h-6 text-white"
                  fill="none"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="3"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path d="M5 13l4 4L19 7" />
                </svg>
              </div>
            </div>
          )}
        </div>
      </div>
    </Card>
  )
}
