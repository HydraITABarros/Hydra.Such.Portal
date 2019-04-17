// https://github.com/diegohaz/arc/wiki/Atomic-Design
import React from 'react'

import {
    PageTemplate, Organism, Button
} from 'components'

const OrdensDeManutencao = () => {
    return (
        <PageTemplate
            header={<div>Header</div>}
            hero={<div>Hero</div>}
            footer={<div>Footer</div>} >
            <div> Page Content </div>
        </PageTemplate>
    )
}

export default OrdensDeManutencao
