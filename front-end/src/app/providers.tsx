'use client';

import { Toaster } from 'sonner';
import { TanstackQueryProvider } from 'src/contexts/tanstack-query-provider';

export function Providers({ children }: { children: React.ReactNode }) {
    return (
        <TanstackQueryProvider>
            {children}
            <Toaster />
        </TanstackQueryProvider>
    );
}
