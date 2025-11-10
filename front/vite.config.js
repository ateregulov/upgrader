import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'
import { viteCommonjs } from '@originjs/vite-plugin-commonjs'
import { NodeGlobalsPolyfillPlugin } from '@esbuild-plugins/node-globals-polyfill'
import { NodeModulesPolyfillPlugin } from '@esbuild-plugins/node-modules-polyfill'

export default defineConfig({
  define: {
    'process.env': {},
    global: 'globalThis',
  },
  plugins: [
    react({
      fastRefresh: true,
    }),
    viteCommonjs({
      include: ['TronWeb', 'node_modules'],
      transformMixedEsModules: true,
      skipPreBuild: true,
      requireReturnsDefault: 'auto',
      defaultIsModuleExports: 'auto',
    }),
    {
      name: 'provide-node-polyfills',
      configureServer(server) {
        server.middlewares.use((req, res, next) => {
          if (req.url.startsWith('/node_modules')) {
            res.setHeader('Content-Type', 'application/javascript')
          }
          next()
        })
      },
    },
  ],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src'), // Use "@" as an alias for src directory

      stream: 'stream-browserify',
      zlib: 'browserify-zlib',
      util: 'util',
      assert: 'assert',
      http: 'stream-http',
      https: 'https-browserify',
      os: 'os-browserify',
      url: 'url',
    },
  },
  optimizeDeps: {
    include: [
      'stream-browserify',
      'assert',
      'http',
      'https',
      'os',
      'url',
    ],
    esbuildOptions: {
      define: {
        global: 'globalThis',
      },
      plugins: [
        NodeGlobalsPolyfillPlugin({
          process: true,
          buffer: true,
          crypto: true,
        }),
        NodeModulesPolyfillPlugin(),
      ],
    },
  },
  build: {
    commonjsOptions: {
      transformMixedEsModules: true,
      sourcemap: true,
      include: [/TronWeb/, /node_modules/],
      requireReturnsDefault: 'auto',
      defaultIsModuleExports: 'auto',
    },
    rollupOptions: {
      external: ['@TronWeb'],
      output: {
        entryFileNames: 'assets/[name].[hash].js',
        chunkFileNames: 'assets/[name].[hash].js',
        assetFileNames: 'assets/[name].[hash].[ext]'
      }
    },
    target: 'es2020',
    minify: false,
  },
  server: {
    port: 5444,
    open: true,
    hmr: {
      overlay: true,
    },
    watch: {
      usePolling: true,
      interval: 1000,
    },
  },
})
