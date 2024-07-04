module.exports = {
  content: [
    './src/{pages,layout,components}/**/*.tsx',
    './src/{pages,layout,components}/**/*.ts',
    './src/styles/**/*.css',
    './src/styles/**/*.scss',
  ],
  theme: {
    extend: {
      height: {
        page: 'calc(100vh - 72px)'
      },
      fontFamily: {
        'mono': ['Ubunto Mono', 'monospace'],
      }
    }
  },
  plugins: [],
}
