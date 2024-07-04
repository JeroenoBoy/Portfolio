import '../styles/globals.scss'
import type { AppProps } from 'next/app'
import Navbar from '../components/Navbar'
import {FC, PropsWithChildren} from 'react';
import DefaultLayout from '../components/layouts/Default';
import Footer from '../components/Footer';
import '@fortawesome/fontawesome-svg-core/styles.css';
import {ScrollAgentProvider} from "../components/hooks/ScrollAgent";


export type Page = FC & {
  layout?: FC<PropsWithChildren<{}>>
}


function MyApp({ Component, pageProps }: AppProps & {  Component: Page }) {
  const Layout = Component.layout || DefaultLayout as FC

  return (
    <div>
      <div className='min-h-screen'>
        <ScrollAgentProvider>
          <Navbar />
          <Layout>
            <Component {...pageProps} />
          </Layout>
        </ScrollAgentProvider>
      </div>
      
      <Footer />
    </div>
  )
}


export default MyApp