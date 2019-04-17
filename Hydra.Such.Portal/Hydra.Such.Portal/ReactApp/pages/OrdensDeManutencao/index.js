// https://github.com/diegohaz/arc/wiki/Atomic-Design
import React from 'react'

import {
    PageTemplate, Header, Hero, Footer, FeatureList,
} from 'components'

const OrdensDeManutencao = () => {
    return (
        <PageTemplate
            header={<Header />}
            hero={<Hero />}
            footer={<Footer />}
        >
            <FeatureList />
        </PageTemplate>
    )
}

export default OrdensDeManutencao
