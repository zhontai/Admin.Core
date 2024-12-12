import { WebSocketApi } from '/@/api/admin/WebSocket'

export class WebSocketClient {
  private socket: WebSocket | null = null
  private reconnectInterval: number = 5000 // 重连间隔（毫秒）
  private heartbeatInterval: number = 30000 // 心跳间隔（毫秒）
  private heartbeatTimeout: number | null = null
  private reconnectTimer: number | null = null
  private isConnected: boolean = false
  private onMessageCallback: (message: MessageEvent) => void

  constructor(
    options: {
      onMessage?: (message: MessageEvent) => void
      reconnectInterval?: number
      heartbeatInterval?: number
    } = {}
  ) {
    this.onMessageCallback = options.onMessage || (() => {})
    if (options.reconnectInterval) {
      this.reconnectInterval = options.reconnectInterval
    }
    if (options.heartbeatInterval) {
      this.heartbeatInterval = options.heartbeatInterval
    }
    this.init()
  }

  private async init(): Promise<void> {
    const res = await new WebSocketApi().isUseIm({}).catch(() => {})
    if (res?.success && res.data) this.connect()
  }

  private async connect(): Promise<void> {
    const res = await new WebSocketApi().preConnect({}).catch(() => {})
    let url = ''
    if (res?.success) {
      url = res.data.server
    } else {
      this.startReconnect()
      return
    }

    if (!url) {
      this.startReconnect()
      return
    }

    if (this.socket) {
      this.socket.close()
      this.socket = null
    }

    this.socket = new WebSocket(url)

    this.socket.onopen = () => {
      console.log('WebSocket connection opened')
      this.isConnected = true
      this.startHeartbeat()
      this.clearReconnectTimer()
    }

    this.socket.onmessage = (event: MessageEvent) => {
      //console.log('WebSocket message received:', event.data);
      this.onMessageCallback(event)
    }

    this.socket.onclose = () => {
      console.log('WebSocket connection closed')
      this.isConnected = false
      this.stopHeartbeat()
      this.startReconnect()
    }

    this.socket.onerror = (error: Event) => {
      console.error('WebSocket error:', error)
      this.socket?.close()
    }
  }

  private startHeartbeat(): void {
    if (this.heartbeatTimeout) {
      clearTimeout(this.heartbeatTimeout)
    }

    this.heartbeatTimeout = window.setTimeout(() => {
      if (this.isConnected) {
        this.sendHeartbeat()
      }
    }, this.heartbeatInterval)
  }

  private sendHeartbeat(): void {
    if (this.socket && this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(JSON.stringify({ type: 'heartbeat' }))
      this.startHeartbeat()
    }
  }

  private stopHeartbeat(): void {
    if (this.heartbeatTimeout) {
      clearTimeout(this.heartbeatTimeout)
      this.heartbeatTimeout = null
    }
  }

  private startReconnect(): void {
    if (this.reconnectTimer) {
      clearTimeout(this.reconnectTimer)
    }

    this.reconnectTimer = window.setTimeout(async () => {
      console.log('Attempting to reconnect...')
      const res = await new WebSocketApi().isUseIm({}).catch(() => {})
      if (res?.success && res.data) {
        this.connect()
      }
    }, this.reconnectInterval)
  }

  private clearReconnectTimer(): void {
    if (this.reconnectTimer) {
      clearTimeout(this.reconnectTimer)
      this.reconnectTimer = null
    }
  }

  public send(data: any): void {
    if (this.socket && this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(JSON.stringify(data))
    } else {
      console.error('WebSocket is not open. Ready state:', this.socket ? this.socket.readyState : 'null')
    }
  }

  public close(): void {
    this.stopHeartbeat()
    this.clearReconnectTimer()
    if (this.socket) {
      this.socket.close()
    }
  }
}
