import type React from "react"
import type { Metadata } from "next"
import "./globals.css"
import { Providers } from "./providers"

export const metadata: Metadata = {
  title: "Vote agora!",
  description: "Vote no participante que você quer eliminar"
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html lang="pt-BR">
      <body className={`font-sans antialiased`}>
        <Providers>
          <header className="bg-zinc-800 py-3 px-4">
            <div className="max-w-7xl mx-auto flex items-center justify-between">
              <div className="text-white font-bold text-lg md:text-xl">gshow</div>
              <div className="text-white font-bold text-sm md:text-base tracking-wider">BIG BROTHER BRASIL</div>
            </div>
          </header>
          {children}
        </Providers>
      </body>
    </html>
  )
}
