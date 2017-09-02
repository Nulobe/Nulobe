import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { NULOBE_ENV_SETTINGS } from '../../../environments/environment';
import { IPermissionsResolver } from '../../core/abstractions';
import { Fact, VoteApiClient, FlagApiClient } from '../../core/api';
import { AuthService } from '../../features/auth';

@Component({
  selector: 'app-fact-list',
  templateUrl: './fact-list.component.html',
  styleUrls: ['./fact-list.component.scss']
})
export class FactListComponent implements OnInit {
  @Input() facts$: Observable<Fact>;

  private permissionsResolver: IPermissionsResolver;

  constructor(
    private voteApiClient: VoteApiClient,
    private flagApiClient: FlagApiClient,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.permissionsResolver = {
      resolve: () => this.authService.isAuthenticated() 
    };
  }

  navigateToTag(tag: string) {
    this.router.navigate([`q/${tag}/force`]);
  }

  voteFact(fact: Fact) {
    this.voteApiClient.create({ factId: fact.id })
      .subscribe();
  }

  flagFact(fact: Fact) {
    this.flagApiClient.create({ factId: fact.id })
      .subscribe();
  }

  editFact(fact: Fact) {
    this.router.navigate([`/LOBE/admin/edit/${fact.id}`]);
  }

  linkFact(fact: Fact) {
    let factSlugSplit = fact.slug.split('-');
    let factPathComponents = [
      NULOBE_ENV_SETTINGS.baseUrl,
      "n",
      factSlugSplit[0],
      factSlugSplit.slice(1, factSlugSplit.length).join('-')
    ];
    let factUrl = factPathComponents.join('/');
    copyText(factUrl);
  }
}

function createNode(text) {
  var node = document.createElement('textarea');
  node.style.position = 'absolute';
  node.style.fontSize = '12pt';
  node.style.border = '0';
  node.style.padding = '0';
  node.style.margin = '0';
  node.style.left = '-10000px';
  node.style.top = (window.pageYOffset || document.documentElement.scrollTop) + 'px';
  node.textContent = text;
  return node;
}

function copyNode(node) {
  try {
    // Set inline style to override css styles
    document.body.style.webkitUserSelect = 'initial';

    var selection = document.getSelection();
    selection.removeAllRanges();

    var range = document.createRange();
    range.selectNodeContents(node);
    selection.addRange(range);
    // This makes it work in all desktop browsers (Chrome)
    node.select();
    // This makes it work on Mobile Safari
    node.setSelectionRange(0, 999999);

    if (!document.execCommand('copy')) {
      throw ('failure copy');
    }
    selection.removeAllRanges();
  } finally {
    // Reset inline style
    document.body.style.webkitUserSelect = '';
  }
}

function copyText(text) {
  var left = window.pageXOffset || document.documentElement.scrollLeft;
  var top = window.pageYOffset || document.documentElement.scrollTop;

  var node = createNode(text);
  document.body.appendChild(node);
  copyNode(node);

  window.scrollTo(left, top);
  document.body.removeChild(node);
}
