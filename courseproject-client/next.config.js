/** @type {import('next').NextConfig} */
const nextConfig = {
  experimental: {
    appDir: true,
  },
  async rewrites() {
    return [
      {
        source: '/blurb-api/:slug*',
        destination: 'http://localhost:5119/blurb-api/:slug*' // Proxy to Backend
      }
    ]
  }
}

module.exports = nextConfig
