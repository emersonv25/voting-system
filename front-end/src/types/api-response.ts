/**
 * Interface que representa a resposta da API.
 */
export interface ApiResponse<TData> {
  status: number // Status da resposta, como "success" ou "error"
  message?: string // Mensagem de erro ou sucesso
  data?: TData // Dados retornados pela API
}


/**
 * Interface que representa uma resposta de erro da API.
 */
export interface ApiErrorResponse {
  statusCode: number
  message: string
  errors?: string[]
}